import { useEffect, useState } from 'react';
import GroupCard from '@/components/chat/GroupCard';


const Sidebar = () => {
    const [groups, setGroups] = useState([]);
    
    // fake request to get the list of group that the user is part of
    /*
    useEffect(() => {
        const fetchGroups = async () => {
            try {
                const response = await fetch('http://localhost:_/groups');
                setGroups(response.data);
            } catch (error) {
                console.error(error);
            }
        };
        fetchGroups();
    }, []);
    */


  return (
    <div className="w-1/4 bg-white border-r border-gray-300">
      <header className="p-4 border-b border-gray-300 flex justify-between items-center bg-indigo-600 text-white">
        <h1 className="text-2xl font-semibold">Hetic Chat</h1>
      </header>
      <div className="overflow-y-auto h-screen p-3 mb-9 pb-20">
        {
            groups.map((group) => (
                <GroupCard key={group.id} groupName={group.name} groupDate={group.createdAt} />
            ))
        }
        <GroupCard groupName="projet scolaire" groupDate={new Date().toLocaleDateString()} />
        <GroupCard groupName="Alice et Bob" groupDate={new Date().toLocaleDateString()} />
      </div>
    </div>
  );
};

export default Sidebar;
