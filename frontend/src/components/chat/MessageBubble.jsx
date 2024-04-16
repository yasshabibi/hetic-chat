import { getInitials } from "@/tools/function";


const MessageBubble = ({ currentUser, text, userName }) => {
    const messageClass = currentUser ? "flex justify-end mb-4 cursor-pointer" : "flex mb-4 cursor-pointer";
    const bubbleClass = currentUser ? "flex max-w-96 bg-indigo-500 text-white rounded-lg p-3 gap-3" : "flex max-w-96 bg-white rounded-lg p-3 gap-3";
  
    return (
      <div className={messageClass}>
        {!currentUser && (
          <div className="w-9 h-9 rounded-full bg-slate-400 flex items-center justify-center mr-2">
            {getInitials(userName)}
          </div>
        )}
        <div className={bubbleClass}>
          <p>{text}</p>
        </div>
        {currentUser && (
          <div className="w-9 h-9 rounded-full bg-slate-400 flex items-center justify-center ml-2">
            {getInitials(userName)}
          </div>
        )}
      </div>
    );
  };
  
  export default MessageBubble;
  