using System;
using System.Windows.Controls;
using FluentAssertions;
using NUnit.Framework;
using NZazu.Contracts.Checks;

namespace NZazu.Fields
{
    [TestFixture]
    [RequiresSTA]
    // ReSharper disable InconsistentNaming
    class NZazuIntegerField_Should
    {
        [Test]
        public void Be_Creatable()
        {
            var sut = new NZazuIntegerField("test");

            sut.Should().NotBeNull();
            sut.Should().BeAssignableTo<INZazuField>();
        }

        [Test]
        public void Create_TextBox_With_ToolTip_Matching_Description()
        {
            var sut = new NZazuIntegerField("test")
            {
                Hint = "superhero",
                Description = "check this if you are a registered superhero"
            };

            var textBox = (TextBox)sut.ValueControl;
            textBox.Should().NotBeNull();
            textBox.Text.Should().BeEmpty();
            textBox.ToolTip.Should().Be(sut.Description);
        }

        [Test]
        public void Format_Correct_Value()
        {
            var sut = new NZazuIntegerField("test");

            var textBox = (TextBox)sut.ValueControl;

            sut.Value.Should().NotHaveValue();
            textBox.Text.Should().BeEmpty();

            sut.Value = 42;
            textBox.Text.Should().Be("42");

            sut.Value = -23;
            textBox.Text.Should().Be("-23");

            textBox.Text = "7";
            sut.Value.Should().Be(7);

            textBox.Text = "-12";
            sut.Value.Should().Be(-12);

            textBox.Text = "foo bar";
            new Action(sut.Validate).Invoking(a => a()).ShouldThrow<ValidationException>();
            sut.Value.Should().Be(-12, because: "WPF binding cannot sync value");
        }

        [Test]
        public void Format_StringValue()
        {
            var sut = new NZazuIntegerField("test");

            var textBox = (TextBox)sut.ValueControl;

            sut.StringValue.Should().BeNullOrEmpty();
            textBox.Text.Should().BeEmpty();

            sut.StringValue = "42";
            textBox.Text.Should().Be("42");

            sut.StringValue = "-23";
            textBox.Text.Should().Be("-23");

            textBox.Text = "7";
            sut.StringValue.Should().Be("7");

            textBox.Text = "-12";
            sut.StringValue.Should().Be("-12");

            textBox.Text = "foo bar";
            new Action(sut.Validate).Invoking(a => a()).ShouldThrow<ValidationException>();
            sut.StringValue.Should().Be("-12", because: "WPF binding cannot sync value");
        }
    }
}