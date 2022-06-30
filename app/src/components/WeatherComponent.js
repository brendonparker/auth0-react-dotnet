import { useApi } from "../auth0/useApi";
export default function Page1() {
  const { data, loading, refresh, error } = useApi("/weatherforecast");
  const weather = data || [];

  if (loading) {
    return <h1>Loading...</h1>;
  }
  if (error) {
    return (
      <>
        <h1>Errored</h1>
        <div>{error.message}</div>
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
}
