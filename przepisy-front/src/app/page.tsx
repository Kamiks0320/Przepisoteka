import Link from "next/link";

export default async function Home() {
  const res = await fetch("http://localhost:5220/api/recipe", {cache: "no-store"})

  if(!res.ok) return <p>Błąd ładowania</p>

  const recipes = await res.json();

  return (
    <main className="p-6">
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
    </main>
  );
}
