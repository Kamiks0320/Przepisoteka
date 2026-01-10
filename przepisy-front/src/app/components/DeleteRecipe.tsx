"use client";

import { useRouter } from "next/navigation";

type Props = {
    recipeId: string;
}

export default function DeleteRecipe({recipeId}: Props) {
    const router = useRouter();

    async function handleDelete(){

        const res = await fetch(`http://localhost:5220/api/recipe/${recipeId}`, {method:"DELETE"},);
    
    if(!res.ok) {
        alert("Błąd przy usuwaniu");
        return;
    }

    router.push("/");
    }

    return(
        <button onClick={handleDelete}>Usuń przepis</button>
    )
}