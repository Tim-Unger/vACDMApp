import {useEffect, useState} from "react";
import HomeScreen from "@/app/home/homeScreen";
import SettingsScreen from "@/app/settings/settingsScreen";
import {BottomNavigation, Provider, TextInput} from "react-native-paper";
import {Icon} from "react-native-paper/src";
import {Dimensions, StyleSheet, Text, useWindowDimensions, View} from "react-native";
import {SafeAreaView} from "react-native-safe-area-context";
import MyFlightScreen from "@/app/me/myFlightScreen";
import BookmarksScreen from "@/app/bookmarks/bookmarksScreen";
import NoConfigScreen from "@/app/noConfigScreen";
import {AppConfiguration} from "@/types/AppConfiguration";
import {useTheme} from "@react-navigation/core";

interface Route {
    key: string;
    title: string;
    icon: string;
}

export default function Index() {
    const [index, setIndex] = useState(0);
    const [config, setConfig] = useState<AppConfiguration | undefined>(undefined);
    const theme = useTheme();
    const [cid, setCid] = useState<string>();
    const [isCidError, setCidError] = useState<boolean>(false);

    useEffect(() => {
        const fetchConfig = async () => {
            try {
                const configResponse = await fetch('config.json');

                const config: AppConfiguration | undefined = await configResponse.json();
            } catch {
                return (
                    <NoConfigScreen/>
                )
            }
        }

        fetchConfig();
    }, []);

    if (config === undefined) {
        return (
            <SafeAreaView style={{
                flex: 1,
                marginTop: 50,
                justifyContent: 'flex-start',
            }}>
                <View style={{justifyContent: 'center', alignItems: 'center'}}>
                <Text style={{color: theme.colors.text, fontWeight: "bold", fontSize: 24}}>Please add some information</Text>
                </View>

                <View style={{marginTop: 50, marginLeft: 50}}>
                    <Text style={{color: theme.colors.text, fontSize: 20}}>CID</Text>
                    <TextInput style={{backgroundColor: theme.colors.card, marginTop: 10, marginRight: 50, color: 'green'}} underlineColorAndroid={theme.colors.border} maxLength={8} value={cid} error={isCidError} onChangeText={(text) => {
                        if(text === '') {
                            setCidError(false);
                            setCid('')
                            return;
                        }

                        const numberValue = parseInt(text.trim());
                        if(isNaN(numberValue)) {
                            setCidError(true);
                            setCid(text.trim());
                            return;
                        }

                        setCidError(false);
                        setCid(text.trim());
                    }}/>
                </View>
            </SafeAreaView>
        )
    }

    const routes: Route[] = [
        {key: 'me', title: 'My Flight', icon: 'account'},
        {key: 'flights', title: 'Flights', icon: 'airplane'},
        {key: 'bookmarks', title: 'Bookmarks', icon: 'bookmark'},
        // {key: 'ecfmp', title: 'ECFMP', icon: 'alert-circle'},
        {key: 'settings', title: 'Settings', icon: 'cog'},
    ];

    const renderScene = ({route}: { route: Route }) => {
        switch (route.key) {
            case 'me':
                return <MyFlightScreen/>;
            case 'flights':
                return <HomeScreen/>;
            case 'bookmarks':
                return <BookmarksScreen/>;
            case 'settings':
                return <SettingsScreen/>;
            default:
                return null;
        }
    };

    return (
        <SafeAreaView style={{
            flex: 1
        }}>
            <Provider>
                {renderScene({route: routes[index]})}
                <BottomNavigation.Bar
                    style={{
                        marginTop: 'auto',
                    }}
                    navigationState={{index, routes}}
                    onTabPress={({route}) => {
                        const newIndex = routes.findIndex((r) => r.key === route.key);
                        if (newIndex !== -1) {
                            setIndex(newIndex);
                        }
                    }}
                    renderIcon={({route, color}) => (
                        <Icon source={route.icon} size={24} color={color}/>
                    )}
                    getLabelText={({route}) => route.title}
                />
            </Provider>
        </SafeAreaView>
    );
}
