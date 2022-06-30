import { useEffect, useState } from "react";
import { getWeather } from "./Api";
export default function Page1() {
  const [weather, setWeather] = useState([]);
  useEffect(() => {
    (async () => {
      setWeather(await getWeather());
    })();
  }, []);

  return (
    <>
      <h1>Weather</h1>
      {weather.map((x) => (
        <p key={x.date}>
          {x.temperatureF} {x.summary}
        </p>
      ))}
    </>
  );
}
