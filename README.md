Hello.

This is a readme file of an NeatMenu project.

NeatMenu consists of classes that had been designed to help people to create a neat console menus. :-)

The main idea is a represence of an interactive suit for some objects that are *subj*s. This suit works with controllers (not mvc actually, but still good)
that are meant to realize some logic and output separately. Another cool with thing with NeatMenu is that it has a delayed input processor which made possible an easy
and observable conditional blocks and navigation to create. And inbuilt Pager for element blocks!

Also it serves an example how lots of basic stuff are currently missed in netcore and free linux in 2021 unfortunately (remember, console is the main interaction
instrument in linux). A lack of ConsoleKey(Modifier) describer, strange Console.Clear() approach are the proofs.. I do believe omissions will be overcomed some day!

Now, to code:
NeatMenu structure can be described in a short list:
NeatMenu (BasicImages\NeatMenu.cs) - the core that has some override possible not-very-meaningful attributes, which may be ignored. Also it has some base classes
that are applied almost everywhere in NeatMenu Controllers. See file NeatMenu.cs for additional description.
SimpleArithmeticsMenu - a descendant of basic NeatMenu class. It describes a walkpaths that are depending on Controller's Results.
PageContollers - the illustration and incarnation of main idea (seems cool to me); usually is consisting of three parts: Controller, Options and Result.
Conn - is a console utility class.
SimpleArithmetics - example of NeatMenu possible subj, i.e. an object to build a menu round about.

NeatMenu project is free of use (CPOL). I will be glad if while using NeatMenu you will keep a small credits to an author in commentaries.

Also remember, that gratitude feels good, while currency supports good. :)

If you like NeatMenu so much or it did come handy and saved you a lot of time (nerves), you may provide some support for me (appreciate for anything)
with Qiwi:
+7(nine-zero-four)6543257

Thanks for visiting and reading. Have a good luck!
