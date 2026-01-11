"use client";

import {useRouter} from "next/navigation";
import { useState } from "react";

export default function AddRecipeForm() {
    const router = useRouter();

    const [name, setName] = useState<string>("");
    const [description, setDescription] = useState<string>("");
    const [ingredients, setIngredients] = useState<string>("");
    const [error, setError] = useState<string | null>(null);

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();

        //walidacja na froncie
        if(name.trim().length === 0) {
            setError("Nazwa przepisu jest wymagana");
            return;
        }
        if(description.length === 0) {
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

        const res = await fetch(`http://localhost:5220/api/recipe/`, {
            method: "POST",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify({name, description, ingredientNames: ingredientList,})
        })

        if (!res.ok) {
            const text = await res.text();
            setError(`Błąd backendu: ${res.status} - ${text}`);
            return;
        }

        router.push(`/`);
    }

    return (
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
                placeholder="składniki oddziel przecinkami" 
            />

            {error && (
                <p className="text-red-600 font-medium">
                    {error}
                </p>
            )}
            
            <button className="bg-blue-600 px-4 py-2 rounded text-white">Dodaj</button>
        </form>
    )
}