import { useState, useEffect } from 'react';

import MessageBubble from '@/components/chat/MessageBubble';


const groupName = "Alice et Bob";
const userId = 3;

const ChatArea = () => {
    const [groupMessages, setGroupMessages] = useState([]);

    // simulate request to get the messages of the group by group id

    /*
    useEffect(() => {
        const fetchMessages = async () => {
            try {
                const response = await fetch(`http://localhost:_/groups/${groupId}/messages`);
                setGroupMessages(response.data);
            } catch (error) {
                console.error(error);
            }
        };
        fetchMessages();
    });
    */

    return (
        <div className="flex-1">
            <header className="bg-white p-4 text-gray-700 border-b">
                <h1 className="text-2xl font-semibold">{groupName}</h1>
            </header>
            <div className="h-screen bg-slate-300 overflow-y-auto p-4 pb-36">
                {groupMessages.map((message) => (
                    <MessageBubble key={message.id} currentUser={message.user.id === userId} text={message.text} userName={message.user.name} />
                ))}

                <MessageBubble currentUser={false} text="Hey Bob, how's it going?" userName="Alice" />
                <MessageBubble currentUser={true} text="Hi Alice! I'm good, just finished a great book. How about you?" userName="Bob" />
            </div>
            <footer className="bg-white border-t border-gray-300 p-4 absolute bottom-0 w-3/4">
                <div className="flex items-center">
                    <input type="text" placeholder="Type a message..." className="w-full p-2 rounded-md border border-gray-400 focus:outline-none focus:border-blue-500" />
                    <button className="bg-indigo-500 text-white px-4 py-2 rounded-md ml-2">Send</button>
                </div>
            </footer>
        </div>
    );
};

export default ChatArea;
