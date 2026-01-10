import Link from "next/link";
import DeleteRecipe from "../../components/DeleteRecipe";

type PageProps = {
  params: Promise< {
    id: string;
  }>;
};

export default async function RecipePage({ params }: PageProps) {
    const {id} = await params;
    const res = await fetch(`http://localhost:5220/api/recipe/${id}`, {cache: "no-store"})

    if(!res.ok) return <p>Błąd {res.status}</p>

    const recipe = await res.json();

    return (
        <main className="p-6">
            <p>{recipe.description}</p>
            <ul>
                {recipe.ingredients.map((i: string) => (
                    <li key={i}>{i}</li>
                ))}
            </ul>
            <DeleteRecipe recipeId = {recipe.id} />
            <Link href={`/recipes/${recipe.id}/edit`} className="">Edytuj</Link>
        </main>
  );
}
