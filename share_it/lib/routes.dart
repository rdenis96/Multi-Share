import 'package:flutter/material.dart';
import 'package:share_it/screens/home/home.dart';
import 'package:share_it/screens/launch/launch.dart';
import 'package:share_it/utils/common/constants/common_constants.dart';

class MyApp extends StatelessWidget {
  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'ShareIt',
      theme: ThemeData(primarySwatch: CustomColors.materialMainColor),
      initialRoute: RoutesConstants.launch,
      routes: {
        RoutesConstants.launch: (context) => Launch(),
        RoutesConstants.home: (context) => Home()
        // RoutesConstants.signIn: (context) => SignIn(),
        // RoutesConstants.signUp: (context) => SignUp()
      },
    );
  }
}