"use client"

import Link from "next/link"
import { useAuth } from "@/hooks/useAuth"
import DeleteRecipe from "./DeleteRecipe"

export default function UmbrellaDeleteEditRecipe({recipeId, ownerId}: {recipeId: string; ownerId: string;}) {
    const { userId, roles } = useAuth();
    const isOwner = userId === ownerId;
    const isAdmin = roles.includes("Administrator");
    const isModerator = roles.includes("Moderator");
    console.log("Uprawnienia:", { userId, ownerId, roles, isOwner, isAdmin, isModerator });

    if(!(isOwner || isAdmin || isModerator)) return null;

    return(
        <div>
            <Link href={`/recipes/${recipeId}/edit`}>Edytuj</Link>
            <DeleteRecipe recipeId={recipeId} />
        </div>
    )
}