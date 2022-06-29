import "./App.css";
import { BrowserRouter, Routes, Route } from "react-router-dom";

function Page1() {
  return <h1>Page 1</h1>;
}

function Page2() {
  return <h1>Page 2</h1>;
}

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Page1 />} />
        <Route path="/page2" element={<Page2 />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
