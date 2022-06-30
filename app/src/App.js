import "./App.css";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import WeatherComponent from "./WeatherComponent";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<WeatherComponent />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
