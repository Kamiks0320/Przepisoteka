import AddRecipeForm from "@/app/components/AddRecipeForm";

export default function AddRecipePage() {
    return (
        <main className="p-6 max-w-xl">
              <h1 className="text-xl mb-4">Dodaj przepis</h1>
        
              <AddRecipeForm/>
            </main>
    )
}