# Automate IE #

**Automate IE** is a simplified automation tool for testing web front-ends using Internet Explorer.  It uses code based on the [IE Driver](http://www.codeproject.com/Articles/9683/Automating-Internet-Explorer) projcet from Code Project to automate an IE window&mdash;not an embedded IE control.
Rather than being code driven, such as [Selenium](http://seleniumhq.org/), the tests are XML documents with an XSD (see sample) that will allow VS2010 to autocomplete the elements and attributes.

## Development Requirements ##
- Visual Studio 2010, C#, .NET Framework 3.5.
- Internet Explorer 8.0 (not tested with 9.0)
- [iC#Code](http://www.icsharpcode.net/OpenSource/SD/Default.aspx) TextEditor v4.2

## TODO ##
- Add timings to know how long a step takes
- Add loops to repeat certain steps
- Add better error handling to clean up objects, reattach, and alllow re-running of a script

## Known Issues ##
The automation can be a bit flakey&mdash;so using something like [Selenium](http://seleniumhq.org/) may be a better choice.  To give the app the best chance of automating, it is normally best to start IE on the first page to test.  Doing so often helps the automation be successful.

Also, when unknown errors occur, it is best to close the application and start it up again&mdash;need to sort out reattaching to the current IE instance.

## License ##

Released under the [MIT License](http://www.opensource.org/licenses/mit-license.php)

Copyright (c) 2012 Doug Thompson

Permission is hereby granted, free of charge, to any person obtaining a
copy of this software and associated documentation files (the
"Software"),to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be included
in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
