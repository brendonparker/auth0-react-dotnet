import { NavLink } from "react-router-dom";
export default function NavBar() {
  return (
    <ul>
      <li>
        <NavLink to="profile">Profile</NavLink>
      </li>
      <li>
        <NavLink to="/">Weather</NavLink>
      </li>
    </ul>
  );
}
