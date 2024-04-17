import { useState } from 'react';
import { createGroup } from '@/api/group';

  const fakeUsers = [
    {
        "id": 3,
        "username": "samuel",
        "usernameSafe": "samuel",
        "email": "samuel@gmail.com",
        "passwordHash": ""
      },
      {
        "id": 4,
        "username": "michel",
        "usernameSafe": "michel",
        "email": "michel@gmail.com",
        "passwordHash": ""
      },
      {
        "id": 5,
        "username": "richard",
        "usernameSafe": "richard",
        "email": "richard@gmail.com",
        "passwordHash": ""
      }
]

const GroupCreation = () => {
    const [users, setUsers] = useState([]);
    const [selectedUsers, setSelectedUsers] = useState([]);
    const [searchTerm, setSearchTerm] = useState('');

    // useEffect(() => {
    //     // Fetch users data from API
    //     const fetchUsers = async () => {
    //         try {
    //             const response = await fetch('https://jsonplaceholder.typicode.com/users');
    //             setUsers(response.data);
    //         } catch (error) {
    //             console.error(error);
    //         }
    //     };

    //     fetchUsers();
    // }, []);

    const handleCheckboxChange = (userId) => {
        if (selectedUsers.includes(userId)) {
            setSelectedUsers(selectedUsers.filter((id) => id !== userId));
        } else {
            setSelectedUsers([...selectedUsers, userId]);
        }
    };

    const filteredUsers = fakeUsers.filter((user) =>
        user.username.toLowerCase().includes(searchTerm.toLowerCase())
    );    
    const handleSubmit = async (e) => {
        e.preventDefault();
        console.log('Selected users:', selectedUsers);

        // Call API to create group
        const groupData = {
            name: e.target.groupName.value,
            createdBy: 1, // Hardcoded for now
            memberUserIds: selectedUsers,
        };

        try {
            const response = await createGroup(groupData);
            console.log('Group created:', response);
            // Handle success
        } catch (error) {
            console.error('Error creating group:', error);
            // Handle error
        }
    }

    return (
        <div className="w-full h-full flex">
            <div className="w-8/12 p-4">
                <div className="mb-4">
                    <input
                        type="text"
                        placeholder="Search users"
                        value={searchTerm}
                        onChange={(e) => setSearchTerm(e.target.value)}
                        className="border border-gray-300 rounded px-4 py-2 w-full"
                    />
                </div>
                <div className="border border-gray-300 rounded p-2 h-full overflow-y-auto">
                    {filteredUsers.map((fakeUsers) => (
                        <div key={fakeUsers.id} className="flex items-center mb-2 w-full min-h-12  px-4 py-2 border border-gray-300 rounded">
                            <input
                                type="checkbox"
                                checked={selectedUsers.includes(fakeUsers.id)}
                                onChange={() => handleCheckboxChange(fakeUsers.id)}
                                className="mr-2"
                            />
                            <span>{fakeUsers.username}</span>
                        </div>
                    ))}
                </div>
            </div>
            <div className="w-4/12 p-4">
                <div className="border border-gray-300 rounded px-4 py-2">
                    <form>
                        <div className="mb-4">
                            <label htmlFor="groupName" className="block mb-2">
                                Group Name
                            </label>
                            <input
                                type="text"
                                id="groupName"
                                className="border border-gray-300 rounded px-4 py-2 w-full"
                            />
                        </div>
                        <div className="mb-4 text-sm text-gray-600">
                            number of users selected: {selectedUsers.length}
                        </div>
                        <button
                            onClick={handleSubmit}
                            type="submit"
                            className="bg-blue-500 text-white px-4 py-2 rounded"
                        >
                            Create Group
                        </button>
                    </form>
                </div>
            </div>
        </div>
    );
};

export default GroupCreation;