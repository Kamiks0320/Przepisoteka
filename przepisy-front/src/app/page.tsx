import Link from "next/link";
import AddRecipeButton from "./components/AddRecipeButton";
const API_URL = process.env.INTERNAL_API_URL!;

export default async function Home() {
  
  console.log("API URL =", API_URL);
  const [recipeRes, ingredientRes] = await Promise.all([
    fetch(API_URL +"/api/recipe", {cache: "no-store"}),
    fetch(API_URL +"/api/ingredient", {cache: "no-store"})])
  
  
  if(!recipeRes.ok || !ingredientRes.ok) return (
  <p>Błąd ładowania lub brak przepisów!
  <AddRecipeButton /></p>
)

  const recipes = await recipeRes.json();
  const ingredients = await ingredientRes.json();

  return (
    <main className="p-5 grid grid-cols-2 gap-5">
      <section>
        <h1 className="text-2xl font-bold mb-4">Przepisy kulinarne:</h1>

        <ul className="space-y-2">
          {recipes.map((recipe: any) => (
            <li key={recipe.id} className="border p-4 rounded">
              <h2 className="text-lg font-semibold">
                {recipe.name}
              </h2>
              <Link href={`/recipes/${recipe.id}`} className="">Szczegóły</Link>
            </li>
          ))}
        </ul>
        <AddRecipeButton />
      </section>

      <section>
        <h1 className="text-2xl font-bold mb-4">Składniki w bazie danych:</h1>

        <ul className="space-y-1">
          {ingredients.map((ingredient: any) => (
            <li key={ingredient.id} className="border p-1 rounded">
              <h2 className="text-lg font-semibold">
                {ingredient.name}
              </h2>
            </li>
          ))}
        </ul>
      </section>
    </main>
  );
}
