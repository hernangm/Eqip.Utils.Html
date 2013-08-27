using System;
using Eqip.Utils.Html;
using NUnit.Framework;

namespace Eqip.Utils.Html.tests
{
    public class UnitTest1
    {
        [Test]
        public void TestMethod1()
        {
            var expected = "Introducción: ";
//            var test = @"<p><strong>Introducci&oacute;n</strong></p>
//<p>La aparici&oacute;n anual en los &uacute;ltimos a&ntilde;os de casos aut&oacute;ctonos de Dengue en el &aacute;rea urbana de responsabilidad program&aacute;tica de la instituci&oacute;n, lleva a la realizaci&oacute;n de&nbsp; acciones de foco conjuntamente con acciones de prevenci&oacute;n. La vigilancia epidemiol&oacute;gica y sus acciones derivadas se convirtieron en una prioridad de la pr&aacute;ctica institucional.</p>
//<p>&nbsp;</p>
//<p><strong>Objetivo</strong></p>
//<p>Describir el resultado de las acciones realizadas ante la aparici&oacute;n de casos sospechosos de Dengue.</p>
//<p>&nbsp;</p>
//<p><strong>Material y m&eacute;todos</strong></p>
//<p>El presente trabajo es un estudio transversal y descriptivo. Se utiliz&oacute; una encuesta como instrumento de recolecci&oacute;n de datos y eventual extracci&oacute;n de sangre y an&aacute;lisis de serolog&iacute;a y tipificaci&oacute;n de virus, siendo la poblaci&oacute;n estudiada aquella que&nbsp;habita en un radio de 9 manzanas &nbsp;que rodean a los casos &iacute;ndice sospechosos de dengue. Se toc&oacute; timbre en los domicilios dentro del &aacute;rea geogr&aacute;fica definida. En el caso de respuesta, se realizaron las preguntas definidas en el instrumento. Si existiese un caso con los criterios de s&iacute;ndrome febril, fue citado al hospital para la extracci&oacute;n de sangre. &nbsp;En una de las zonas investigadas se realiz&oacute; la extraccion en el lugar. &nbsp;</p>
//<p>En &nbsp;domicilios con respuesta se realizaron acciones informativas y &nbsp;preventivas, en los cerrados se dejo material informativo.</p>
//<p>&nbsp;</p>
//<p><em>Per&iacute;odo de estudio</em>: Los 4 casos estudiados tuvieron lugar entre marzo y mayo del 2012. Cada uno &nbsp;llev&oacute; entre 2 y seis d&iacute;as de relevamiento. &nbsp;</p>
//<p>&nbsp;</p>
//<p><em>Variables medidas</em></p>
//<p>Domicilio: Locaci&oacute;n del timbre tocado por el equipo de salud. En el caso de asentamientos se realiz&oacute; una localizaci&oacute;n geogr&aacute;fica de las unidades habitacionales. No se incluye la metodolog&iacute;a por cuestiones de espacio</p>
//<p>Tipo de casa, definida por el encuestador. Valores posibles: casa, bald&iacute;o o comercio</p>
//<p>Situaci&oacute;n frente al timbrado: presencia o ausencia de respuesta</p>
//<p>N&ordm; de casos febriles sospechosos de dengue&nbsp;encontrados de acuerdo a la definici&oacute;n de caso vigente (actuales o en los 30 d&iacute;as anteriores a la encuesta).&nbsp;</p>
//<p>N&ordm; de personas citadas que efectivamente concurrieron a la extracci&oacute;n de sangre en el hospital</p>
//<p>Se obtuvieron medidas de frecuencia y proporciones de las mismas. Los domicilios fueron georreferenciados.&nbsp;</p>
//<p>&nbsp;</p>
//<p><strong>Resultados</strong></p>
//<p>Se toc&oacute; timbre en 2207 domicilios. La tabla 1 resume las unidades relevadas y los casos probables encontrados.</p>
//<table border=""1"" cellspacing=""0"" cellpadding=""0"" width=""100%"">
//  <tr>
//    <td valign=""top""><p>Área</p></td>
//    <td valign=""top""><p>Casas    totales</p></td>
//    <td valign=""top""><p>Casas    cerradas</p></td>
//    <td valign=""top""><p>Casas    renuentes</p></td>
//    <td valign=""top""><p>Casas ingresadas<br />
//        n (%)</p></td>
//    <td valign=""top""><p>Casos    febriles</p></td>
//    <td valign=""top""><p>Muestra de sangre obtenida<br />
//        n (% de los febriles)</p></td>
//    <td valign=""top""><p>Casos    probables</p></td>
//  </tr>
//  <tr>
//    <td valign=""top""><p>1</p></td>
//    <td valign=""top""><p>886</p></td>
//    <td valign=""top""><p>357</p></td>
//    <td valign=""top""><p>26</p></td>
//    <td valign=""top""><p>479    (54,06)</p></td>
//    <td valign=""top""><p>15</p></td>
//    <td valign=""top""><p>8    (53)</p></td>
//    <td valign=""top""><p>6</p></td>
//  </tr>
//  <tr>
//    <td valign=""top""><p>2</p></td>
//    <td valign=""top""><p>305</p></td>
//    <td valign=""top""><p>139</p></td>
//    <td valign=""top""><p>0</p></td>
//    <td valign=""top""><p>166 (54,42)</p></td>
//    <td valign=""top""><p>5</p></td>
//    <td valign=""top""><p>4    (80)</p></td>
//    <td valign=""top""><p>2</p></td>
//  </tr>
//  <tr>
//    <td valign=""top""><p>3</p></td>
//    <td valign=""top""><p>276</p></td>
//    <td valign=""top""><p>138</p></td>
//    <td valign=""top""><p>8</p></td>
//    <td valign=""top""><p>130    (47,1)</p></td>
//    <td valign=""top""><p>0</p></td>
//    <td valign=""top""><p>-</p></td>
//    <td valign=""top""><p>-</p></td>
//  </tr>
//  <tr>
//    <td valign=""top""><p>4</p></td>
//    <td valign=""top""><p>740</p></td>
//    <td valign=""top""><p>331</p></td>
//    <td valign=""top""><p>4</p></td>
//    <td valign=""top""><p>409    (55,27)</p></td>
//    <td valign=""top""><p>3</p></td>
//    <td valign=""top""><p>1    (66,6)</p></td>
//    <td valign=""top""><p>0</p></td>
//  </tr>
//  <tr>
//    <td valign=""top""><p>Total</p></td>
//    <td valign=""top""><p>2207</p></td>
//    <td valign=""top""><p>965</p></td>
//    <td valign=""top""><p>38</p></td>
//    <td valign=""top""><p>1184    (53,5)</p></td>
//    <td valign=""top""><p>23</p></td>
//    <td valign=""top""><p>13    (56)</p></td>
//    <td valign=""top""><p>8</p></td>
//  </tr>
//</table>
//<p><strong>Conclusiones</strong></p>
//<p>Se detectaron casos probables de dengue. La proporci&oacute;n de casas ingresadas fue adecuada a los standares, as&iacute; como la concurrencia luego de la citaci&oacute;n. Se pudieron instrumentar medidas preventivas y de autocuidado.&nbsp;</p>
//<p>La infecci&oacute;n por dengue muestra paulatinamente un comportamiento end&eacute;mico en la ciudad, con caracter&iacute;sticas estacionales anuales. Creemos que es fundamental el fortalecimiento de la vigilancia epidemiol&oacute;gica conjunta con las acciones en salud p&uacute;blica para esta patolog&iacute;a en el contexto de los equipos locales.&nbsp;</p>";
            var toreplace = @"<header class=""abstract-header""><h3 class=""abstract-title"">Tipo de alimentación de los niños hasta el mes de edad.</h3><ol class=""abstract-authors""><li class=""first"">DC Caceres<sup class=""abstract-institution-reference"">1</sup></li><li>MV Ferrin<sup class=""abstract-institution-reference"">2</sup></li><li>MC Delgadillo<sup class=""abstract-institution-reference"">3</sup></li></ol><ol class=""abstract-institutions""><li><sup class=""institution-order"">1</sup>&nbsp;U. S. Molina Campos, Municipio de Moreno, Argentina</li><li><sup class=""institution-order"">2</sup>&nbsp;U. S. Reja Centro, Municipio de Moreno, Argentina</li><li><sup class=""institution-order"">3</sup>&nbsp;Maternidad Ramon Sarda, Argentina</li></ol></header>";
            var replaced = toreplace.AsHtmlString().RemoveComments().RemoveJavaScript().RemoveNonPrintableCharacters().StripTags().RemoveMultipleSpaces().ToString();
            Assert.AreEqual(expected, replaced);
        }
    }
}
