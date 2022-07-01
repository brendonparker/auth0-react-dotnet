import "./App.css";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import WeatherComponent from "./components/WeatherComponent";
import Auth0Provider from "./auth0/Auth0Provider";
import ProtectedRoute from "./auth0/ProtectedRoute";
import Profile from "./components/Profile";
import NavBar from "./components/NavBar";
import UsersComponent from "./components/UsersComponent";
import NewUserComponent from "./components/NewUserComponent";

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
          <Route
            path="/users"
            element={<ProtectedRoute component={UsersComponent} />}
          />
          <Route
            path="/user/create"
            element={<ProtectedRoute component={NewUserComponent} />}
          />
        </Routes>
      </Auth0Provider>
    </BrowserRouter>
  );
}

export default App;
