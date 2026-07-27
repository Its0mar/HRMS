import { zodResolver } from "@hookform/resolvers/zod";
import { useState } from "react";
import { useForm } from "react-hook-form";
import z from "zod";

const LoginFormValues = z.object({
    "identifier" : z.string().min(3).max(30),
    "password" : z.string().min(8).max(30)
});

export type LoginFormValues = z.infer<typeof LoginFormValues>;

export function LoginForm() {

    const [isLoading, setIsLoading] = useState(false);

    const form = useForm<LoginFormValues>({
        resolver : zodResolver(LoginFormValues),
        defaultValues : {
            "identifier" : "",
            "password" : ""
        }
    });

    const { register } = form;
    
    const handleSubmit = (data : LoginFormValues) => {
            console.log(data);
            setIsLoading(true);
        }

    return (
        <div className="flex min-h-screen w-full flex-col justify-center overflow-auto px-6 py-12 lg:px-8">
            <div className="sm:mx-auto sm:w-full sm:max-w-sm">
                <h2 className="mt-10 text-center text-2xl/9 font-bold tracking-tight text-gray-900 dark:text-white">Login to your account</h2>
            </div>
            
            {/* {globalError && (
                <p style={{ color: "#d32f2f", padding: "10px", backgroundColor: "#ffebee", borderRadius: "4px", marginBottom: "15px" }}>
                    {globalError}
                </p>
            )} */}

            <div className="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
                <form className="w-full space-y-6" onSubmit={form.handleSubmit(handleSubmit)}>
                    <div>
                        <label className="block text-sm/6 font-medium text-gray-900 dark:text-gray-100">Idntifier</label>
                        <div className="mt-2">
                            <input 
                            className= "block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6 dark:bg-white/5 dark:text-white dark:outline-white/10 dark:placeholder:text-gray-500 dark:focus:outline-indigo-500"
                            type="text" 
                            {...register("identifier")} 
                            placeholder="Enter Idntifier" 
                            disabled={isLoading}
                        />
                        </div>
                    </div>

                     <div>
                        <label className="block text-sm/6 font-medium text-gray-900 dark:text-gray-100">Password</label>
                        <div className="mt-2">
                            <input 
                            className= "block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6 dark:bg-white/5 dark:text-white dark:outline-white/10 dark:placeholder:text-gray-500 dark:focus:outline-indigo-500"
                            type="password" 
                            {...register("password")} 
                            placeholder="Enter Password" 
                            disabled={isLoading}
                        />
                        </div>
                    </div>

                    <button 
                        className="flex w-full justify-center rounded-md bg-indigo-600 px-3 py-1.5 text-sm/6 font-semibold text-white shadow-xs hover:bg-indigo-500 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600 dark:bg-indigo-500 dark:shadow-none dark:hover:bg-indigo-400 dark:focus-visible:outline-indigo-500"
                        type="submit" 
                        disabled={isLoading}>
                        {isLoading ? "Login..." : "Login"}
                    </button>

                </form>

                <p className="mt-10 text-center text-sm/6 text-gray-500 dark:text-gray-400">
                    Not a member?
                    <a href="#" className="font-semibold text-indigo-600 hover:text-indigo-500 dark:text-indigo-400 dark:hover:text-indigo-300 ml-1">Create account</a>
                </p>
            </div>

        </div>
    )
}