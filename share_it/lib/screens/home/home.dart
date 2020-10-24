import 'package:argon_buttons_flutter/argon_buttons_flutter.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_spinkit/flutter_spinkit.dart';

class Home extends StatefulWidget {
  @override
  _HomeState createState() => _HomeState();
}

class _HomeState extends State<Home> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(),
      body: new Column(
        children: <Widget>[
          Row(
            children: [
              Container(
                  child: ArgonButton(
                height: 50,
                width: 350,
                borderRadius: 5.0,
                color: Color(0xFF7866FE),
                child: Text(
                  "Continue",
                  style: TextStyle(
                      color: Colors.white,
                      fontSize: 18,
                      fontWeight: FontWeight.w700),
                ),
                loader: Container(
                  padding: EdgeInsets.all(10),
                  child: SpinKitRing(
                    color: Colors.red,
                    size: 30,
                  ),
                ),
                onTap: (startLoading, stopLoading, btnState) {
                  if (btnState == ButtonState.Busy) {
                    stopLoading.call();
                  } else {
                    startLoading.call();
                  }
                },
              ))
            ],
          ),
          Row(
            children: [
              Container(
                  child: RaisedButton(
                child: new Text("Facebook app is not installed!"),
                onPressed: () => {},
              ))
            ],
          )
        ],
      ),
    );
  }
}
