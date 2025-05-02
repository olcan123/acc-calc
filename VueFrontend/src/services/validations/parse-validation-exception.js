function parseValidationException(errorMessage) {
    const lines = errorMessage
      .split('\n')
      .filter(line => line.trim().startsWith('--'));
  
    return lines.map(line => {
      const match = line.match(/--\s*(\w+):\s*(.*?)\.\s*Severity/i);
      if (match) {
        return {
          field: match[1].trim(),
          message: match[2].trim()
        };
      }
      return null;
    }).filter(Boolean);
  }
  
  export default parseValidationException;  


  //NOTE - Bunu vue-toastification'da kullanıyoruz
  //NOTE - Ilerde gerekirse fonksiyonnun adini degistiriyoruz.
  // function parseValidationException(errorMessage) {
  //   const lines = errorMessage
  //     .split('\n')
  //     .filter(line => line.trim().startsWith('--'));
  
  //   return lines.map(line => {
  //     const match = line.match(/--\s*(\w+):\s*(.*?)\.\s*Severity/i);
  //     if (match) {
  //       return `${match[1]}: ${match[2]}`;
  //     }
  //     return null;
  //   }).filter(Boolean);
  // }
  
  // export default parseValidationException;  