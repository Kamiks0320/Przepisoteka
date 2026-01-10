"use client";

import {useRouter} from "next/navigation";
import {useState} from "react";

type Props = {
    recipe: any;
};

export default function EditRecipeForm({recipe}: Props) {
    const router = useRouter();

    //stale do przechwycenia i 
    const [name, setName] = useState(recipe.name);
    const [description, setDescription] = useState(recipe.description);
    const [ingredients, setIngredients] = useState(recipe.ingredients.join(", "));

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();

        await fetch(`http://localhost:5220/api/recipe/${recipe.id}`, {
            method: "PUT",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify({name, description, ingredientNames: ingredients.split(","),}),
        });

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

            <button className="bg-blue-600 px-4 py-2 rounded text-white">Zapisz</button>
        </form>
    )
}