import { useEffect, useState } from "react";
import { getWeather } from "./Api";
export default function Page1() {
  const [weather, setWeather] = useState({});
  useEffect(() => {
    (async () => {
      setWeather(await getWeather());
    })();
  }, []);

  return (
    <>
      <h1>Page 1</h1>
      {JSON.stringify(weather)}
    </>
  );
}
