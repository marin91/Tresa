using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tresa.Models
{
    public class AppSettings : INotifyPropertyChanged
    {
        private double _edgeSensitivity = 0.5; // 0..1
        private string _colorMode = "Grayscale"; // or "Contrast"
        private string _exportFormat = "PNG"; // or "SVG"


        public event PropertyChangedEventHandler? PropertyChanged;


        public double EdgeSensitivity { get => _edgeSensitivity; set { _edgeSensitivity = value; PropertyChanged?.Invoke(this, new(nameof(EdgeSensitivity))); } }
        public string ColorMode { get => _colorMode; set { _colorMode = value; PropertyChanged?.Invoke(this, new(nameof(ColorMode))); } }
        public string ExportFormat { get => _exportFormat; set { _exportFormat = value; PropertyChanged?.Invoke(this, new(nameof(ExportFormat))); } }
    }
}
