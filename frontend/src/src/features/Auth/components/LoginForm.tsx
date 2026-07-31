import { Card, Title, Text, TextInput, Button, Stack, PasswordInput, Notification, Anchor  } from "@mantine/core";
import { useForm } from '@mantine/form';
import { zod4Resolver } from "mantine-form-zod-resolver";
import z from "zod";
import { apiClient } from "../../../lib/apiClient";
import { API_ROUTES } from "../../../lib/apiRoutes";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import { XIcon } from '@phosphor-icons/react';
import { useAuthStore } from "../../../store/useAuthStore";


const schema = z.object({
    "identifier" : z.string().min(3).max(100),
    "password" : z.string().min(8).max(30)
});

export type LoginFormValues = z.infer<typeof schema>;

export function LoginForm() {

    const navigate = useNavigate();

    const [isLoading, setIsLoading] = useState(false);
    const [globalError, setGlobalError] = useState<string | null>(null);
    const setSession = useAuthStore((state) => state.setSession);
    const xIcon = <XIcon size={20} />;

    const form = useForm({
        validate : zod4Resolver(schema),
        initialValues : {
            "identifier" : "",
            "password" : ""
            },
    });

    const handleSubmit = async (values: LoginFormValues) => {
        setIsLoading(true);
        setGlobalError(null);

        try {
            const response = await apiClient.post(
                API_ROUTES.AUTH.LOGIN,
                values
            );

            const {user, accessToken} = response.data;

            if (!user || !accessToken) {
            setGlobalError("The server returned an invalid login response.");
            return;
            }

            setSession(user, accessToken);
            navigate("/departments", { replace: true });
        } catch (error) {
            if (axios.isAxiosError(error)) {
                const message =
                    error.response?.data?.errors?.[0]?.description;

                setGlobalError(message ?? "Unable to sign in.");
            } else {
                setGlobalError("An unexpected error occurred.");
            }
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="flex min-h-screen items-center justify-center px-4">
            <Card
                w="100%"
                maw={420}
                padding="xl"
                radius="lg"
                shadow="xl"
                bg="gray.0"
            >
                <Stack gap="lg">
                    <div className="text-center">
                        <Title order={2}>Welcome back</Title>

                        <Text c="dimmed" size="sm" mt={4}>
                            Sign in to continue to HRMS
                        </Text>
                    </div>

                     {globalError && (
                        <Notification onClose={() => setGlobalError(null)} icon={xIcon} color="red" title="Login failed">
                            {globalError}
                        </Notification>
                    )}

                    <form onSubmit={form.onSubmit(handleSubmit)}>
                        <Stack gap="md">
                            <TextInput
                                withAsterisk
                                label="Identifier"
                                placeholder="Email or username"
                                key={form.key("identifier")}
                                {...form.getInputProps("identifier")}
                            />

                            <PasswordInput
                                withAsterisk
                                label="Password"
                                placeholder="Enter your password"
                                key={form.key("password")}
                                {...form.getInputProps("password")}
                            />

                            <Button type="submit" fullWidth mt="sm" loading={isLoading}>
                                Sign in
                            </Button>
                        </Stack>
                    </form>
                    <Text className="!items-center !justify-center">
                        Dont have an account?{" "}
                        <Anchor
                            component="button"
                            type="button"
                            onClick={() => navigate("/register", { replace: true })}
                        >
                            Sign up
                        </Anchor>
                    </Text>
                </Stack>
            </Card>
        </div>
    );
}
