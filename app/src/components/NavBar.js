import { NavLink } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";

export default function NavBar() {
  const { user, loginWithRedirect, logout } = useAuth0();
  return (
    <nav className="navbar">
      <ul className="nav-links">
        <div className="menu">
          <li>
            <NavLink to="profile">Profile</NavLink>
          </li>
          <li>
            <NavLink to="/">Weather</NavLink>
          </li>
          <li>
            <NavLink to="/users">Users</NavLink>
          </li>
          {!user ? (
            <li>
              <button className="link" onClick={() => loginWithRedirect()}>
                Log In
              </button>
            </li>
          ) : (
            <li>
              <button className="link" onClick={() => logout()}>
                Log Out
              </button>
            </li>
          )}
        </div>
      </ul>
    </nav>
  );
}
