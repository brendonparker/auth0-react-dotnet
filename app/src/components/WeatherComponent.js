import { useApi } from "../auth0/useApi";
const WeatherComponent = () => {
  const { data, loading, refresh, error } = useApi("/weatherforecast");
  const weather = data || [];

  if (loading) {
    return <h1>Loading...</h1>;
  }
  if (error) {
    return (
      <>
        <h1>Weather</h1>
        <h2>Errored</h2>
        <h3>{error.message}</h3>
      </>
    );
  }
  return (
    <>
      <h1>Weather</h1>
      {weather.map((x) => (
        <p key={x.date}>
          {x.temperatureF} {x.summary}
        </p>
      ))}
      <button onClick={refresh}>Refresh</button>
    </>
  );
};

export default WeatherComponent;
