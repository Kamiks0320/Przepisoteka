"use client";

import {useRouter} from "next/navigation";
import {useState} from "react";

type Props = {
    recipe: any;
};

export default function EditRecipeForm({recipe}: Props) {
    const router = useRouter();

    const [name, setName] = useState<string>(recipe.name);
    const [description, setDescription] = useState<string>(recipe.description);
    const [ingredients, setIngredients] = useState<string>(recipe.ingredients.join(", "));
    const [error, setError] = useState<string | null>(null)

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();

        //walidacja na froncie
        if(name.trim().length === 0) {
            setError("Nazwa przepisu jest wymagana");
            return;
        }
        if(description.trim().length === 0) {
            setError("Opis jest wymagany");
            return;
        }
        const ingredientList = ingredients
            .split(",")
            .map(i => i.trim())
            .filter(i => i.length > 0);
        if(ingredientList.length === 0) {
            setError("Co najmniej jeden składnik jest wymagany");
            return;
        }

        const res = await fetch(`http://localhost:5220/api/recipe/${recipe.id}`, {
            method: "PUT",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify({name, description, ingredientNames: ingredients.split(","),}),
        });

        if (!res.ok) {
            const text = await res.text();
            setError(`Błąd backendu: ${res.status} - ${text}`);
            return;
        }

        router.push(`/recipes/${recipe.id}`);
    }

    return(
        <form onSubmit={handleSubmit} className="space-y-4 text-black">
            <input 
                value={name} 
                onChange={e => setName(e.target.value)} 
                className="border p-2 w-full" 
            />

            <textarea 
                value={description} 
                onChange={e => setDescription(e.target.value)} 
                className="border p-2 w-full" 
            />
            <input 
                value={ingredients} 
                onChange={e => setIngredients(e.target.value)} 
                className="border p-2 w-full" 
                placeholder="składniki po przecinku" 
            />

            {error && (
                <p className="text-red-600 font-medium">
                    {error}
                </p>
            )}

            <button className="bg-blue-600 px-4 py-2 rounded text-white">Zapisz</button>
        </form>
    )
}