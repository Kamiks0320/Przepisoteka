import EditRecipeForm from "@/app/components/EditRecipeForm";
const API_URL = process.env.INTERNAL_API_URL!;

type PageProps = {
    params: Promise<{id: string}>;
};

export default async function EditRecipePage({ params }: PageProps) {
  const { id } = await params;

  const res = await fetch(`${API_URL}/api/recipe/${id}`, {
    cache: "no-store",
  });

  if (!res.ok) return <p>Błąd ładowania</p>;

  const recipe = await res.json();

  return (
    <main className="p-6 max-w-xl">
      <h1 className="text-xl mb-4">Edytuj przepis</h1>

      <EditRecipeForm recipe={recipe} />
    </main>
  );
}