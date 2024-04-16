CREATE TABLE IF NOT EXISTS `Users` (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL,
    password_hash VARCHAR(255) NOT NULL
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS `Groups` (
    group_id INT AUTO_INCREMENT PRIMARY KEY,
    group_name VARCHAR(255) NOT NULL,
    created_by INT NOT NULL,
    FOREIGN KEY (created_by) REFERENCES `Users`(user_id)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS GroupMembers (
    group_id INT,
    user_id INT,
    role ENUM('admin', 'member') NOT NULL,
    PRIMARY KEY (group_id, user_id),
    FOREIGN KEY (group_id) REFERENCES `Groups`(group_id),
    FOREIGN KEY (user_id) REFERENCES `Users`(user_id)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS Messages (
    message_id INT AUTO_INCREMENT PRIMARY KEY,
    group_id INT NOT NULL,
    user_id INT NOT NULL,
    content TEXT NOT NULL,
    timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (group_id) REFERENCES `Groups`(group_id),
    FOREIGN KEY (user_id) REFERENCES `Users`(user_id)
) ENGINE=InnoDB;
