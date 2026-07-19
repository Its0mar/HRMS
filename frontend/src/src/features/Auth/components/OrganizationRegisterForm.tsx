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
    "logoUrl" : z.url("invalid link")
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
            "logoUrl" : ''
        }
    })

    const handleSubmit = (data : OrganizationRegisterFormValues) => {
        console.log(data);
        setIsLoading(true);
    }

    const { register } = form;

    return (
       <div >
            <h2>Register</h2>
            
            {/* {globalError && (
                <p style={{ color: "#d32f2f", padding: "10px", backgroundColor: "#ffebee", borderRadius: "4px", marginBottom: "15px" }}>
                    {globalError}
                </p>
            )} */}
            
            <form onSubmit={form.handleSubmit(handleSubmit)}>
                <div>
                    <label>Organization Name</label>
                    <input 
                        type="text" 
                        {...register("organizationName")} 
                        placeholder="Enter Organization Name" 
                        disabled={isLoading}
                    />
                </div>

                <div>
                    <label>Organization Code</label>
                    <input 
                        type="text" 
                        {...register("organizationCode")} 
                        placeholder="Enter Organization Code" 
                        disabled={isLoading}
                    />
                </div>
                
                <div>
                    <label>Organization Email</label>
                    <input 
                        type="text" 
                        {...register("organizationEmail")} 
                        placeholder="Enter Organization Email" 
                        disabled={isLoading}
                    />
                </div>

                <div>
                    <label>Organization Address</label>
                    <input 
                        type="text" 
                        {...register("address")} 
                        placeholder="Enter Organization Address" 
                        disabled={isLoading}
                    />
                </div>

                <div>
                    <label>Organization Website</label>
                    <input 
                        type="text" 
                        {...register("organizationName")} 
                        placeholder="Enter Organization Website" 
                        disabled={isLoading}
                    />
                </div>

                <div>
                    <label>Organization Logo</label>
                    <input 
                        type="file" 
                        {...register("organizationName")} 
                        disabled={isLoading}
                    />
                </div>
                
                <button 
                    type="submit" 
                    disabled={isLoading}
                    // style={{ 
                    //     width: "100%", 
                    //     padding: "10px", 
                    //     // backgroundColor: isLoading ? "#ccc" : "#1976d2", 
                    //     color: "white", 
                    //     border: "none", 
                    //     borderRadius: "4px", 
                    //     fontWeight: "bold", 
                    //     cursor: isLoading ? "not-allowed" : "pointer" 
                    // }}
                >
                    {isLoading ? "Registering..." : "Register"}
                </button>
            </form>
        </div>
    )
}