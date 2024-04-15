import { NavLink } from "react-router-dom";

export default function Navbar() {
   return (
      <nav id="header" className="w-full z-30 py-1 bg-white shadow-lg border-b border-blue-400 top-0">
         <div className="w-full flex items-center justify-between mt-0 px-6 py-2">
            <div className="flex  w-auto order-3 " id="menu">
               <nav>
                  <div className="flex items-center justify-between text-base text-blue-600 pt-0">
                     <NavLink className="inline-block no-underline hover:text-black font-medium text-lg py-2 px-4 lg:-ml-2" to="/">Home</NavLink>
                     <NavLink className="inline-block no-underline hover:text-black font-medium text-lg py-2 px-4 lg:-ml-2" to="/">Users</NavLink>
                  </div>
               </nav>
            </div>

            <div className="order-3 flex flex-wrap items-center justify-end mr-0 md:mr-4" id="nav-content">
               <div className="auth flex items-center w-full md:w-full">
                  <button className="bg-transparent text-gray-800  p-2 rounded border border-gray-300 mr-4 hover:bg-gray-100 hover:text-gray-700">
                     <NavLink to="/login">Login</NavLink>
                  </button>
                  <button className="bg-blue-600 text-gray-200  p-2 rounded  hover:bg-blue-500 hover:text-gray-100">
                     <NavLink to="/register">Sign up</NavLink>
                  </button>
               </div>
            </div>
         </div>
      </nav>
   );
}
