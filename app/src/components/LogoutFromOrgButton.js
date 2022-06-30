import React, { useContext } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { OrgContext } from "../auth0/Auth0Provider";
const LogoutButton = () => {
  const { logout } = useAuth0();
  const { clear } = useContext(OrgContext);

  const onClick = () => {
    clear();
    logout({ returnTo: window.location.origin });
  };

  return <button onClick={onClick}>Log Out from Org</button>;
};

export default LogoutButton;
