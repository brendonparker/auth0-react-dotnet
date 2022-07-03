import { NavLink } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";

export default function NavBar() {
  const { user, loginWithRedirect, logout } = useAuth0();
  return (
    <div className="ui menu">
      <NavLink to="profile" className="item">
        Profile
      </NavLink>
      <NavLink to="/" className="item">
        Weather
      </NavLink>
      <NavLink to="/users" className="item">
        Users
      </NavLink>
      <div className="menu right">
        {!user ? (
          <a href="#" className="item" onClick={() => loginWithRedirect()}>
            Log In
          </a>
        ) : (
          <a href="#" className="item" onClick={() => logout()}>
            Log Out
          </a>
        )}
      </div>
    </div>
  );
}
