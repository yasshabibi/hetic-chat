import { useEffect } from "react";

import Sidebar from "@/components/chat/Sidebar";
import ChatArea from "@/components/chat/ChatArea";

import { getUserData } from "@/api/user";

const Home = () => {

    useEffect(() => {

      const userId = localStorage.getItem('userId');
    //     // Fetch users data from API
        const fetchCurrentUserData = async () => {
          try {
            const response = await getUserData(userId);
            console.log('User data:', response);
            // Handle success
              } catch (error) {
                  console.error('Error fetching user data:', error);
                  // Handle error
              }
        }

        fetchCurrentUserData();
    }, []);

    return (
      <div className="flex h-screen overflow-hidden">
        <Sidebar/>
        <ChatArea/>
      </div>
    );
  };
export default Home;