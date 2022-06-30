import { NavLink } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";

export default function NavBar() {
  const { user, loginWithRedirect, logout } = useAuth0();
  return (
    <nav class="navbar">
      <ul class="nav-links">
        <div class="menu">
          <li>
            <NavLink to="profile">Profile</NavLink>
          </li>
          <li>
            <NavLink to="/">Weather</NavLink>
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
