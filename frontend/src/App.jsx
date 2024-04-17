import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Navbar from '@/components/Navbar';
import Home from '@/components/page/Home';
import Login from '@/components/page/Login';
import Register from '@/components/page/Register';
import GroupCreation from '@/components/page/GroupCreation';

function App() {
  return (
    <Router>
      <div className="w-full h-full d-flex justify-content-center align-items-center">
        <Navbar />
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/group-creation" element={<GroupCreation />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
