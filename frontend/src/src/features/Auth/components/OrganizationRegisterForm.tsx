import { Card, Title, Text, TextInput, Button, Stack, PasswordInput, Notification, SimpleGrid, Divider, Anchor } from "@mantine/core";
import { useForm } from "@mantine/form";
import { XIcon } from "@phosphor-icons/react";
import { zod4Resolver } from "mantine-form-zod-resolver";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import z from "zod";
import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";
import axios from "axios";

const schema = z.object({
    "organizationName" : z.string().min(3).max(30),
    "organizationCode" : z.string().min(3).max(10),
    "organizationEmail" : z.email("invalid email"),
    "address" : z.string().min(3).max(100),
    "website" : z.union([z.url("invalid link"), z.literal("")]),
    "logoUrl" : z.union([z.url("invalid link"), z.literal("")]),

    "ownerUsername" : z.string().min(3).max(30),
    "ownerEmail" : z.email("invalid email"),
    "password" : z.string().min(8).max(30),
    confirmPassword: z.string().min(1, "Please confirm your password"),
    "firstName" : z.string().min(3).max(30),
    "lastName" : z.string().min(3).max(30)
}).refine(
        (values) => values.password === values.confirmPassword,
        {
            message: "Passwords do not match",
            path: ["confirmPassword"]
        }
    );


export type OrganizationRegisterFormValues = z.infer<typeof schema>;


export function OrganizationRegisterForm() {

    const navigate = useNavigate();
    const [isLoading, setIsLoading] = useState(false);
    const [globalError, setGlobalError] = useState<string | null>(null);
    const xIcon = <XIcon size={20} />;

    const form = useForm<OrganizationRegisterFormValues>({
        validate : zod4Resolver(schema),
        initialValues : {
            "organizationName" : "",
            "organizationCode" : "",
            "organizationEmail" : "",
            "address" : "", 
            "website" : "", 
            "logoUrl" : '',
            "ownerUsername" : "",
            "ownerEmail" : "",
            "password" : "",
            "confirmPassword" : "",
            "firstName" : "",
            "lastName" : ""
        }
    })

    const handleSubmit = async (data: OrganizationRegisterFormValues) => {

        const request: Partial<OrganizationRegisterFormValues> = {
            ...data,
            website: data.website || undefined,
            logoUrl: data.logoUrl || undefined
        };
        delete request.confirmPassword;
        setIsLoading(true);
        setGlobalError(null);

        try {
            await apiClient.post(
                API_ROUTES.AUTH.REGISTER,
                request
            );

            navigate("/login", { replace: true });
        } catch (error) {
            if (axios.isAxiosError(error)) {
                const message =
                    error.response?.data?.errors?.[0]?.description;

                setGlobalError(message ?? "Unable to register.");
            } else {
                setGlobalError("An unexpected error occurred.");
            }
        } finally {
            setIsLoading(false);
        }
    }

    return (
    <div className="flex min-h-screen items-center justify-center px-4 py-10">
        <Card
            w="100%"
            maw={760}
            padding="xl"
            radius="lg"
            shadow="xl"
            bg="gray.0"
        >
            <Stack gap="xl">
                <div className="text-center">
                    <Title order={2}>Create your organization</Title>

                    <Text c="dimmed" size="sm" mt={4}>
                        Set up your organization and administrator account
                    </Text>
                </div>

                {globalError && (
                    <Notification
                        icon={xIcon}
                        color="red"
                        title="Registration failed"
                        withCloseButton
                        onClose={() => setGlobalError(null)}
                    >
                        {globalError}
                    </Notification>
                )}

                <form onSubmit={form.onSubmit(handleSubmit)}>
                    <Stack gap="lg">
                        <div>
                            <Text fw={600} mb="sm">
                                Organization information
                            </Text>

                            <SimpleGrid cols={{ base: 1, sm: 2 }}>
                                <TextInput
                                    withAsterisk
                                    label="Organization name"
                                    placeholder="Acme Corporation"
                                    {...form.getInputProps("organizationName")}
                                />

                                <TextInput
                                    withAsterisk
                                    label="Organization code"
                                    placeholder="ACME"
                                    {...form.getInputProps("organizationCode")}
                                />

                                <TextInput
                                    withAsterisk
                                    type="email"
                                    label="Organization email"
                                    placeholder="contact@acme.com"
                                    {...form.getInputProps("organizationEmail")}
                                />

                                <TextInput
                                    withAsterisk
                                    label="Address"
                                    placeholder="Amman, Jordan"
                                    {...form.getInputProps("address")}
                                />

                                <TextInput
                                    label="Website"
                                    placeholder="https://acme.com"
                                    {...form.getInputProps("website")}
                                />

                                <TextInput
                                    label="Logo URL"
                                    placeholder="https://acme.com/logo.png"
                                    {...form.getInputProps("logoUrl")}
                                />
                            </SimpleGrid>
                        </div>

                        <Divider />

                        <div>
                            <Text fw={600} mb="sm">
                                Administrator account
                            </Text>

                            <SimpleGrid cols={{ base: 1, sm: 2 }}>
                                <TextInput
                                    withAsterisk
                                    label="First name"
                                    placeholder="Joe"
                                    {...form.getInputProps("firstName")}
                                />

                                <TextInput
                                    withAsterisk
                                    label="Last name"
                                    placeholder="Doe"
                                    {...form.getInputProps("lastName")}
                                />

                                <TextInput
                                    withAsterisk
                                    label="Username"
                                    placeholder="joe.doe"
                                    {...form.getInputProps("ownerUsername")}
                                />

                                <TextInput
                                    withAsterisk
                                    type="email"
                                    label="Email"
                                    placeholder="joe@acme.com"
                                    {...form.getInputProps("ownerEmail")}
                                />

                                <PasswordInput
                                    withAsterisk
                                    label="Password"
                                    placeholder="At least 8 characters"
                                    {...form.getInputProps("password")}
                                />

                                <PasswordInput
                                    withAsterisk
                                    label="Confirm password"
                                    placeholder="At least 8 characters"
                                    {...form.getInputProps("confirmPassword")}
                                />
                            </SimpleGrid>
                        </div>

                        <Button
                            type="submit"
                            fullWidth
                            size="md"
                            loading={isLoading}
                        >
                            Create organization
                        </Button>
                    </Stack>
                    <Text className="!items-center !justify-center">
                        Already have an account?{" "}
                        <Anchor
                            component="button"
                            type="button"
                            onClick={() => navigate("/login")}
                        >
                            Sign in
                        </Anchor>
                    </Text>
                </form>
            </Stack>
        </Card>
    </div>
);
};
