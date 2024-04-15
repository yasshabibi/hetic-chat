using Backend.Models.Configuration;
using MySqlConnector;

namespace Backend.Managers;

public class DatabaseManager
{
  public string Server { get; init; }
  public string Database { get; init; }
  public string Username { get; init; }
  public string Password { get; init; }
  public string ConnectionString => $"server={Server};database={Database};uid={Username};pwd={Password};";
  public MySqlConnection Connection { get; init; }
  public MySqlDataReader? Reader { get; set; }
  public MySqlTransaction? Transaction { get; set; }

  public DatabaseManager()
  {
    var configuration = new ServerConfiguration();
    Server = configuration.Server;
    Database = configuration.Database;
    Username = configuration.Username;
    Password = configuration.Password;
    Connection = new(ConnectionString);
  }

  public DatabaseManager(string server, string database, string username, string password)
  {
    Server = server;
    Database = database;
    Username = username;
    Password = password;
    Connection = new(ConnectionString);
  }

  /// <summary>
  /// Open a connection, fetch one row, then close the connection, return return a dictionnary of <see cref="object"/>
  /// </summary>
  /// <param name="query">the SQL query</param>
  /// <param name="parameters">parameters to sanitize your request</param>
  /// <param name="user"></param>
  /// <returns>the row returned</returns>
  public async Task<Dictionary<string, object>?> FetchOne(string query, IDictionary<string, object>? parameters = null)
  {
    return await FetchOne<object>(query, parameters);
  }

  /// <summary>
  /// Open a connection, fetch one row, then close the connection. return a dictionnary of <typeparamref name="T"/> 
  /// </summary>
  /// <typeparam name="T">If your fetch is sure to contain a single type, it will cast the whole dictionnary to this type, if multiple types are neede, must be <see cref="object"/>, or <seealso cref="FetchOne(string, IEnumerable{object})"/></typeparam>
  /// <param name="query">the SQL query</param>
  /// <param name="parameters">parameters to sanitize your request</param>
  /// <returns>the row returned</returns>
  public async Task<Dictionary<string, T>?> FetchOne<T>(string query, IDictionary<string, object>? parameters = null)
  {
    await OpenReader(query, parameters);
    Dictionary<string, T> result = new();
    if (!Reader!.HasRows)
    {
      await Reader.CloseAsync();
      await Connection.CloseAsync();
      Reader = null;
      return null;
    }

    await Reader.ReadAsync();

    for (int i = 0; i < Reader.FieldCount; i++)
      result[Reader.GetName(i)] = (T) Reader.GetValue(i);
    

    await Reader.CloseAsync();
    await Connection.CloseAsync();
    Reader = null;
    return result;
  }

  /// <summary>
  /// Fetch a row of an already Openned Reader
  /// </summary>
  /// <returns>the row returned</returns>
  public async Task<Dictionary<string, object>?> FetchOne()
  {
    return await FetchOne<object>();
  }

  /// <summary>
  /// Fetch a row of an already Openned Reader
  /// </summary>
  /// <typeparam name="T">If your fetch is sure to contain a single type, it will cast the whole dictionnary to this type, if multiple types are neede, must be <see cref="object"/>, or <seealso cref="FetchOne()"/></typeparam>
  /// <returns></returns>
  public async Task<Dictionary<string, T>?> FetchOne<T>()
  {
    EnsureReaderOpenned();
    bool t = await Reader!.ReadAsync();
    if (!t)
      return null;
    Dictionary<string, T> result = new();
    for (int i = 0; i < Reader!.FieldCount; i++) 
      result[Reader.GetName(i)] = (T) Reader.GetValue(i);

    return result;
  }

  /// <summary>
  /// Open a connection, fetch every row, then close the connection. This can be really Memory Heavy for big database, consider 
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="query"></param>
  /// <param name="parameters"></param>
  /// <returns></returns>
  public async Task<List<Dictionary<string, T>>> FetchAll<T>(string query, IDictionary<string, object>? parameters = null)
  {
    await OpenReader(query, parameters);
    List<Dictionary<string, T>> result = new();
    if (!Reader!.HasRows)
    {
      await CloseReader();
      return result;
    }

    while (await Reader.ReadAsync())
    {
      Dictionary<string, T> row = new();

      result.Add(row);
    }

    await CloseReader();
    return result;
  }

  public async Task Execute(string query, IDictionary<string, object>? parameters = null)
  {
    await Connection.OpenAsync();
    MySqlCommand command = new MySqlCommand(query, Connection, Transaction);
    if (parameters != null)
      command.Parameters.AddRange(parameters.ToArray());
    await command.ExecuteNonQueryAsync();
    await Connection.CloseAsync();
  }

  /// <summary>
  /// Executes the query, and returns the first column of the first row in the result set returned by the query.
  /// Additional columns or rows are ignored.
  /// </summary>
  /// <param name="query">The SQL query to execute.</param>
  /// <param name="parameters">Parameters to sanitize your request.</param>
  /// <returns>The value of the first column of the first row in the result set.</returns>
  public async Task<object?> ExecuteScalar(string query, IDictionary<string, object>? parameters = null)
  {
      object? result = null;
      await Connection.OpenAsync();
      try
      {
          MySqlCommand command = new MySqlCommand(query, Connection);
          if (parameters != null)
          {
              foreach (var param in parameters)
              {
                  command.Parameters.AddWithValue(param.Key, param.Value);
              }
          }
          result = await command.ExecuteScalarAsync();
      }
      catch (Exception ex)
      {
          // Consider logging the exception or handling it as necessary
          Console.WriteLine($"An error occurred: {ex.Message}");
      }
      finally
      {
          await Connection.CloseAsync();
      }
      return result;
  }

  /// <summary>
  /// Executes an INSERT query and returns the ID of the inserted row.
  /// </summary>
  /// <param name="query">The SQL INSERT query to execute.</param>
  /// <param name="parameters">Parameters to sanitize your request.</param>
  /// <returns>The ID of the newly inserted row as an integer.</returns>
  public async Task<int> ExecuteInsert(string query, IDictionary<string, object>? parameters = null)
  {
      int insertedId = 0;
      await Connection.OpenAsync();
      try
      {
          MySqlCommand command = new MySqlCommand(query, Connection);
          if (parameters != null)
          {
              foreach (var param in parameters)
              {
                  command.Parameters.AddWithValue(param.Key, param.Value);
              }
          }

          await command.ExecuteNonQueryAsync();
          // Assuming auto_increment is enabled and you need to fetch last inserted ID
          command = new MySqlCommand("SELECT LAST_INSERT_ID();", Connection);
          object result = await command.ExecuteScalarAsync();
          insertedId = Convert.ToInt32(result);
      }
      catch (Exception ex)
      {
          // Handle or log the exception as needed
          Console.WriteLine($"An error occurred: {ex.Message}");
      }
      finally
      {
          await Connection.CloseAsync();
      }
      return insertedId;
  }



    public async Task OpenReader(string query, IDictionary<string, object>? parameters = null)
  {
    AssertReaderOpenned();
    await Connection.OpenAsync();
    MySqlCommand command = new MySqlCommand(query, Connection, Transaction);
    if (parameters != null)
      foreach (KeyValuePair<string, object> parameter in parameters)
        command.Parameters.AddWithValue(parameter.Key, parameter.Value);

    Reader = await command.ExecuteReaderAsync();
  }

  public async Task CloseReader()
  {
    EnsureReaderOpenned();
    await Reader!.CloseAsync();
    await Connection.CloseAsync();
  }

  public async Task OpenTransaction()
  {
    AssertTransactionOpenned();
    Transaction = await Connection.BeginTransactionAsync();
  }

  public async Task CommitTransaction()
  {
    EnsureTransactionOpenned();
    await Transaction!.CommitAsync();
  }

  public async Task RollbackTransaction()
  {
    EnsureTransactionOpenned();
    await Transaction!.RollbackAsync();
  }

  #region Security Checks
  private void AssertReaderOpenned()
  {
    if (Connection.State != System.Data.ConnectionState.Closed) throw new InvalidOperationException($"To start a new query, connection must be closed, current state is is {Connection.State}");
  }
  private void EnsureReaderOpenned()
  {
    if (Reader == null || Reader.IsClosed) throw new InvalidOperationException($"Tried to use a closed Reader");
  }
  private void EnsureTransactionOpenned()
  {
    if (Transaction == null) throw new InvalidOperationException($"Tried to close an unexisting Transaction");
  }
  private void AssertTransactionOpenned()
  {
    if (Transaction != null) throw new InvalidOperationException($"Tried to open an already openned Transaction");
  }
  #endregion
}
