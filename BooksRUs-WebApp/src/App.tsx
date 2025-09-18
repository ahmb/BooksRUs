import { Routes, Route, Navigate, NavLink, useLocation } from "react-router-dom";
import Catalog from "./pages/Catalog";
import MyList from "./pages/MyList";
import Header from "./components/Header";

export default function App() {
  const location = useLocation();
  const is = (p: string) => location.pathname.startsWith(p);
  return (
    <div>
      <Header />
      <nav className="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 mt-3 flex gap-3 border-b border-gray-200 pb-4">
        <Tab to="/catalog" active={is("/catalog")}>Catalog</Tab>
        <Tab to="/my-list" active={is("/my-list")}>My List</Tab>
      </nav>
      <main className="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 py-6">
        <Routes>
          <Route path="/" element={<Navigate to="/catalog" replace />} />
          <Route path="/catalog" element={<Catalog />} />
          <Route path="/my-list" element={<MyList />} />
        </Routes>
      </main>
    </div>
  );
}

function Tab({ to, active, children }: { to: string; active: boolean; children: React.ReactNode }) {
  return (
    <>
    <NavLink
      to={to}
      className={`px-3 py-2 rounded-xl text-sm font-medium ${active ? "bg-gray-100 text-black" : "text-gray-600 hover:text-black"}`}
    >
      {children}
    </NavLink>
    </>
  );
}
