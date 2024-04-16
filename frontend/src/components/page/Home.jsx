import Sidebar from "@/components/chat/Sidebar";
import ChatArea from "@/components/chat/ChatArea";

const Home = () => {
    return (
      <div className="flex h-screen overflow-hidden">
        <Sidebar/>
        <ChatArea/>
      </div>
    );
  };
export default Home;