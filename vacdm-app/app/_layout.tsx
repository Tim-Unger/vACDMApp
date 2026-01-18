import {DarkTheme, DefaultTheme, ThemeProvider} from '@react-navigation/native';
import {Stack} from 'expo-router';
import {StatusBar} from 'expo-status-bar';
import 'react-native-reanimated';
import {useColorScheme} from '@/hooks/use-color-scheme';
import {VacdmDarkTheme} from "@/utils/theme";

export default function RootLayout() {
    const colorScheme = useColorScheme();

    return (
        <ThemeProvider value={colorScheme === 'dark' ? VacdmDarkTheme : DefaultTheme}>
            <StatusBar style="auto"/>
            <Stack screenOptions={{headerShown: false}}/>
        </ThemeProvider>
    );
}
