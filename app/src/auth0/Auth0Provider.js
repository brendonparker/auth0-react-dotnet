import { Auth0Provider, useAuth0 } from "@auth0/auth0-react";
import React, { useEffect } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import AUTH0 from "./auth0config";
import useLocal from "../hooks/useLocal";

export const OrgContext = React.createContext({
  org: {},
  setOrg: (org) => {},
  clear: () => {},
});

export default function CustomAuth0Provider({ children }) {
  const navigate = useNavigate();
  const [org, setOrg] = useLocal("demo|org_id");
  const onRedirectCallback = (appState) => {
    navigate((appState && appState.returnTo) || window.location.pathname);
  };

  return (
    <OrgContext.Provider
      value={{
        org,
        setOrg,
        clear: () => localStorage.removeItem("demo|org_id"),
      }}
    >
      <Auth0Provider
        domain={AUTH0.domain}
        clientId={AUTH0.clientId}
        redirectUri={window.location.origin}
        onRedirectCallback={onRedirectCallback}
        audience={AUTH0.audience}
        organization={org?.org_id || undefined}
      >
        <AuthError />
        {children}
        <OrgHack setOrg={setOrg} />
      </Auth0Provider>
    </OrgContext.Provider>
  );
}

function AuthError() {
  const { error } = useAuth0();
  const [searchParams] = useSearchParams();
  if (error) {
    console.log(error);
    return (
      <>
        <h1>Error {error.name}</h1>
        <h2>{error.message}</h2>
        <p>{searchParams.get("error_description")}</p>
      </>
    );
  }
  return <></>;
}

function OrgHack({ setOrg }) {
  const { user } = useAuth0();

  useEffect(() => {
    if (user?.org_id) {
      setOrg({ org_id: user?.org_id });
    }
    // eslint-disable-next-line
  }, [user?.org_id]);

  return <></>;
}
