import { zodResolver } from "@hookform/resolvers/zod";
import { useState } from "react";
import { useForm } from "react-hook-form";
import z from "zod"


const organizationRegisterFormValues = z.object({
    "organizationName" : z.string().min(3).max(30),
    "organizationCode" : z.string().min(3).max(10),
    "organizationEmail" : z.email("invalid email"),
    "address" : z.string().min(3).max(100),
    "website" : z.url("invalid link"),
    "logoUrl" : z.url("invalid link"),

    "ownerUsername" : z.string().min(3).max(30),
    "ownerEmail" : z.email("invalid email"),
    "password" : z.string().min(8).max(30),
    "firstName" : z.string().min(3).max(30),
    "lastName" : z.string().min(3).max(30)

});

export type OrganizationRegisterFormValues = z.infer<typeof organizationRegisterFormValues>;


export function OrganizationRegisterForm() {

    const [isLoading, setIsLoading] = useState(false);

    const form = useForm<OrganizationRegisterFormValues>({
        resolver : zodResolver(organizationRegisterFormValues),
        defaultValues : {
            "organizationName" : "",
            "organizationCode" : "",
            "organizationEmail" : "",
            "address" : "", 
            "website" : "", 
            "logoUrl" : '',
            "ownerUsername" : "",
            "ownerEmail" : "",
            "password" : "",
            "firstName" : "",
            "lastName" : ""
        }
    })

    const handleSubmit = (data : OrganizationRegisterFormValues) => {
        console.log(data);
        setIsLoading(true);
    }

    const { register } = form;

    return (
        
       <div className= "flex min-h-full flex-col justify-center px-6 py-12 lg:px-8">
            <div className="sm:mx-auto sm:w-full sm:max-w-sm">
                <h2 className="mt-10 text-center text-2xl/9 font-bold tracking-tight text-gray-900 dark:text-white">Create a new account</h2>
            </div>
            
            {/* {globalError && (
                <p style={{ color: "#d32f2f", padding: "10px", backgroundColor: "#ffebee", borderRadius: "4px", marginBottom: "15px" }}>
                    {globalError}
                </p>
            )} */}
            
            <div className="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
            <form className="space-y-6" onSubmit={form.handleSubmit(handleSubmit)}>
                {/* Organization Info */}
                <div>
                    <p className="block text-white text-center">Organization Info</p>
                    <div>
                        <label className="block text-sm/6 font-medium text-gray-900 dark:text-gray-100">Organization Name</label>
                        <div className="mt-2">
                            <input 
                            className= "block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6 dark:bg-white/5 dark:text-white dark:outline-white/10 dark:placeholder:text-gray-500 dark:focus:outline-indigo-500"
                            type="text" 
                            {...register("organizationName")} 
                            placeholder="Enter Organization Name" 
                            disabled={isLoading}
                        />
                        </div>
                    </div>

                    <div>
                        <label className="block text-sm/6 font-medium text-gray-900 dark:text-gray-100">Organization Code</label>
                        <div className="mt-2">
                            <input 
                            className= "block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6 dark:bg-white/5 dark:text-white dark:outline-white/10 dark:placeholder:text-gray-500 dark:focus:outline-indigo-500"
                            type="text" 
                            {...register("organizationCode")} 
                            placeholder="Enter Organization Code" 
                            disabled={isLoading}
                        />
                        </div>
                    </div>
                    
                    <div>
                        <label className="block text-sm/6 font-medium text-gray-900 dark:text-gray-100">Organization Email</label>
                        <div className="mt-2">
                            <input 
                            className= "block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6 dark:bg-white/5 dark:text-white dark:outline-white/10 dark:placeholder:text-gray-500 dark:focus:outline-indigo-500"
                            type="text" 
                            {...register("organizationEmail")} 
                            placeholder="Enter Organization Email" 
                            disabled={isLoading}
                        />
                        </div>
                    </div>

                    <div>
                        <label className="block text-sm/6 font-medium text-gray-900 dark:text-gray-100">Organization Address</label>
                        <div className="mt-2">
                            <input 
                            className= "block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6 dark:bg-white/5 dark:text-white dark:outline-white/10 dark:placeholder:text-gray-500 dark:focus:outline-indigo-500"
                            type="text" 
                            {...register("address")} 
                            placeholder="Enter Organization Address" 
                            disabled={isLoading}
                        />
                        </div>
                    </div>

                    <div>
                        <label className="block text-sm/6 font-medium text-gray-900 dark:text-gray-100">Organization Website</label>
                        <div className="mt-2">
                            <input 
                            className= "block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6 dark:bg-white/5 dark:text-white dark:outline-white/10 dark:placeholder:text-gray-500 dark:focus:outline-indigo-500"
                            type="text" 
                            {...register("organizationName")} 
                            placeholder="Enter Organization Website" 
                            disabled={isLoading}
                        />
                        </div>
                    </div>

                    <div>
                        <label className="block text-sm/6 font-medium text-gray-900 dark:text-gray-100">Organization Logo</label>
                        <div className="mt-2">
                            <input 
                            className= "block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6 dark:bg-white/5 dark:text-white dark:outline-white/10 dark:placeholder:text-gray-500 dark:focus:outline-indigo-500"
                            type="file" 
                            {...register("organizationName")} 
                            disabled={isLoading}
                        />
                        </div>
                    </div>
                </div>
                

                {/* owner info */}
                <div>
                    <p className="block text-white text-center">Owner Info</p>
                    <div>
                    <label className="block text-sm/6 font-medium text-gray-900 dark:text-gray-100">Owner Username</label>
                    <div className="mt-2">
                        <input 
                        className= "block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6 dark:bg-white/5 dark:text-white dark:outline-white/10 dark:placeholder:text-gray-500 dark:focus:outline-indigo-500"
                        type="text" 
                        {...register("ownerUsername")} 
                        placeholder="Enter your username" 
                        disabled={isLoading}
                        />
                    </div>
                </div>

                <div>
                    <label className="block text-sm/6 font-medium text-gray-900 dark:text-gray-100">Owner Email</label>
                    <div className="mt-2">
                        <input 
                        className= "block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6 dark:bg-white/5 dark:text-white dark:outline-white/10 dark:placeholder:text-gray-500 dark:focus:outline-indigo-500"
                        type="text" 
                        {...register("ownerEmail")} 
                        placeholder="Enter your email" 
                        disabled={isLoading}
                        />
                    </div>
                </div>

                <div>
                    <label className="block text-sm/6 font-medium text-gray-900 dark:text-gray-100">First name</label>
                    <div>
                        <input 
                        className= "block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6 dark:bg-white/5 dark:text-white dark:outline-white/10 dark:placeholder:text-gray-500 dark:focus:outline-indigo-500"
                        type="text" 
                        {...register("firstName")} 
                        placeholder="Enter your first name" 
                        disabled={isLoading}
                        />
                    </div>
                </div>


                <div>
                    <label className="block text-sm/6 font-medium text-gray-900 dark:text-gray-100">Last name</label>
                    <div className="mt-2">
                        <input 
                        className= "block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6 dark:bg-white/5 dark:text-white dark:outline-white/10 dark:placeholder:text-gray-500 dark:focus:outline-indigo-500"
                        type="text" 
                        {...register("lastName")} 
                        placeholder="Enter your last name" 
                        disabled={isLoading}
                        />
                    </div>
                </div>
                </div>

                <div>
                    <label className="block text-sm/6 font-medium text-gray-900 dark:text-gray-100">Password</label>
                    <div className="mt-2">
                        <input 
                        className= "block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6 dark:bg-white/5 dark:text-white dark:outline-white/10 dark:placeholder:text-gray-500 dark:focus:outline-indigo-500"
                        type="password" 
                        {...register("password")} 
                        placeholder="Enter a password" 
                        disabled={isLoading}
                        />
                    </div>
                </div>


                <button 
                    className="flex w-full justify-center rounded-md bg-indigo-600 px-3 py-1.5 text-sm/6 font-semibold text-white shadow-xs hover:bg-indigo-500 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600 dark:bg-indigo-500 dark:shadow-none dark:hover:bg-indigo-400 dark:focus-visible:outline-indigo-500"
                    type="submit" 
                    disabled={isLoading}
                >
                    {isLoading ? "Registering..." : "Register"}
                </button>
            </form>

            <p className="mt-10 text-center text-sm/6 text-gray-500 dark:text-gray-400">
                already a member?
                <a href="#" className="font-semibold text-indigo-600 hover:text-indigo-500 dark:text-indigo-400 dark:hover:text-indigo-300 ml-1">Login</a>
            </p>

            </div>
        </div>
    )
}