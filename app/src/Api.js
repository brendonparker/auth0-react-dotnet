export { getWeather };
const URL = "https://localhost:7081";

async function getWeather() {
  const res = await fetch(URL + "/weatherforecast", {
    method: "GET",
  });
  const result = await res.json();
  return result;
}
