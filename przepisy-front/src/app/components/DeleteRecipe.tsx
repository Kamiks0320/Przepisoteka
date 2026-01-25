"use client";

import { useRouter } from "next/navigation";

type Props = {
    recipeId: string;
}

export default function DeleteRecipe({recipeId}: Props) {
    const router = useRouter();
    const token = localStorage.getItem("token");

    async function handleDelete(){
        const res = await fetch(`http://localhost:5220/api/recipe/${recipeId}`, {
            method:"DELETE",
            headers: {"Authorization": `Bearer ${token}`}
        },);
    
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