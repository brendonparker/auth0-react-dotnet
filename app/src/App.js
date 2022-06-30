import "./App.css";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import WeatherComponent from "./WeatherComponent";
import Auth0Provider from "./auth0/Auth0Provider";
import ProtectedRoute from "./auth0/ProtectedRoute";
import Profile from "./Profile";
import NavBar from "./NavBar";

function App() {
  return (
    <BrowserRouter>
      <Auth0Provider>
        <NavBar />
        <Routes>
          <Route path="/" element={<WeatherComponent />} />
          <Route
            path="/profile"
            element={<ProtectedRoute component={Profile} />}
          />
        </Routes>
      </Auth0Provider>
    </BrowserRouter>
  );
}

export default App;
