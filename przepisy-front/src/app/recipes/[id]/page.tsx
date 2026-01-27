import UmbrellaDeleteEditRecipe from "@/app/components/UmbrellaDeleteEditRecipe";
const API_URL = process.env.INTERNAL_API_URL!;

type PageProps = {
  params: Promise< {
    id: string;
  }>;
};

export default async function RecipePage({ params }: PageProps) {
    const {id} = await params;
    const res = await fetch(`${API_URL}/api/recipe/${id}`, {cache: "no-store"})

    if(!res.ok) return <p>Błąd {res.status}</p>

    const recipe = await res.json();

    return (
        <main className="p-6">
            <h1 className="bold">{recipe.name}</h1>
            <p>{recipe.description}</p>
            <ul>
                {recipe.ingredients.map((i: string) => (
                    <li key={i}>{i}</li>
                ))}
            </ul>
            <UmbrellaDeleteEditRecipe recipeId={recipe.id} ownerId={recipe.ownerId} />
        </main>
  );
}
