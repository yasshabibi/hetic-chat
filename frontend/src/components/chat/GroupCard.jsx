import { getInitials } from "@/tools/function";

const ContactCard = ({ groupName, groupDate }) => {

    return (
      <div className="flex items-center mb-4 cursor-pointer hover:bg-gray-100 p-2 rounded-md">
        <div className="flex items-center justify-between w-full">
            <div className="flex items-center space-x-4">
                <div className="w-12 h-12 flex items-center justify-center bg-gray-300 text-white rounded-full">
                    {getInitials(groupName)}
                </div>
                <h2 className="text-lg font-semibold">{groupName}</h2>
            </div>
            <p className="text-gray-500">{groupDate}</p>
        </div>
      </div>
    );
  };
  
  export default ContactCard;
  