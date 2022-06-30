import { Auth0Provider } from "@auth0/auth0-react";
import { useNavigate } from "react-router-dom";
import AUTH0 from "./auth0config";

export default function CustomAuth0Provider({ children }) {
  const navigate = useNavigate();
  const onRedirectCallback = (appState) => {
    debugger;
    navigate((appState && appState.returnTo) || window.location.pathname);
  };
  return (
    <Auth0Provider
      domain={AUTH0.domain}
      clientId={AUTH0.clientId}
      redirectUri={window.location.origin}
      onRedirectCallback={onRedirectCallback}
      audience={AUTH0.audience}
      scope="read:users"
    >
      {children}
    </Auth0Provider>
  );
}
